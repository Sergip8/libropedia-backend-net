CREATE TABLE `autores` (
  `id_autor` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `biografia` text DEFAULT NULL,
  `fecha_nacimiento` date DEFAULT NULL,
  `nacionalidad` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_autor`),
  UNIQUE KEY `nombre` (`nombre`,`apellido`),
  KEY `idx_autores_nombre` (`nombre`,`apellido`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


CREATE TABLE `categorias` (
  `id_categoria` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `descripcion` text DEFAULT NULL,
  PRIMARY KEY (`id_categoria`),
  UNIQUE KEY `nombre` (`nombre`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


CREATE TABLE `libros` (
  `id_libro` int(11) NOT NULL AUTO_INCREMENT,
  `titulo` varchar(255) NOT NULL,
  `id_autor` int(11) NOT NULL,
  `id_categoria` int(11) NOT NULL,
  `isbn` varchar(20) DEFAULT NULL,
  `anio_publicacion` year(4) DEFAULT NULL,
  `editorial` varchar(100) DEFAULT NULL,
  `resumen` text DEFAULT NULL,
  `portada_url` varchar(255) DEFAULT NULL,
  `fecha_creacion` datetime DEFAULT current_timestamp(),
  PRIMARY KEY (`id_libro`),
  UNIQUE KEY `isbn` (`isbn`),
  KEY `id_autor` (`id_autor`),
  KEY `idx_libros_titulo` (`titulo`),
  KEY `idx_libros_categoria` (`id_categoria`),
  CONSTRAINT `libros_ibfk_1` FOREIGN KEY (`id_autor`) REFERENCES `autores` (`id_autor`),
  CONSTRAINT `libros_ibfk_2` FOREIGN KEY (`id_categoria`) REFERENCES `categorias` (`id_categoria`)
) ENGINE=InnoDB AUTO_INCREMENT=95 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


CREATE TABLE `resenas` (
  `id_resena` int(11) NOT NULL AUTO_INCREMENT,
  `id_libro` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `calificacion` int(11) NOT NULL CHECK (`calificacion` >= 1 and `calificacion` <= 5),
  `comentario` text DEFAULT NULL,
  `fecha_creacion` datetime DEFAULT current_timestamp(),
  PRIMARY KEY (`id_resena`),
  UNIQUE KEY `id_libro` (`id_libro`,`id_usuario`),
  KEY `id_usuario` (`id_usuario`),
  KEY `idx_resenas_fecha` (`fecha_creacion`),
  CONSTRAINT `resenas_ibfk_1` FOREIGN KEY (`id_libro`) REFERENCES `libros` (`id_libro`) ON DELETE CASCADE,
  CONSTRAINT `resenas_ibfk_2` FOREIGN KEY (`id_usuario`) REFERENCES `usuarios` (`id_usuario`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=94 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


CREATE TABLE `usuarios` (
  `id_usuario` int(11) NOT NULL AUTO_INCREMENT,
  `nombre_usuario` varchar(50) NOT NULL,
  `email` varchar(100) NOT NULL,
  `password` varchar(255) NOT NULL,
  `nombre` varchar(100) DEFAULT NULL,
  `apellido` varchar(100) DEFAULT NULL,
  `fecha_registro` datetime DEFAULT current_timestamp(),
  `ultimo_login` datetime DEFAULT NULL,
  `activo` tinyint(1) DEFAULT 1,
  PRIMARY KEY (`id_usuario`),
  UNIQUE KEY `nombre_usuario` (`nombre_usuario`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=29 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


DELIMITER $$
CREATE DEFINER=`bookstore`@`%` PROCEDURE `sp_registrar_usuario`(
    IN p_nombre_usuario VARCHAR(50),
    IN p_email VARCHAR(100),
    IN p_password VARCHAR(255)
)
BEGIN

    DECLARE hashed_password VARCHAR(255);
    SET hashed_password = SHA2(p_password, 256);
    
    INSERT INTO usuarios (nombre_usuario, email, password)
    VALUES (p_nombre_usuario, p_email, hashed_password);
    
    SELECT LAST_INSERT_ID() AS id_usuario;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`bookstore`@`%` PROCEDURE `sp_autenticar_usuario`(
    IN p_email VARCHAR(50),
    IN p_password VARCHAR(255)
)
BEGIN
    DECLARE hashed_password VARCHAR(255);
    

    SELECT password INTO hashed_password FROM usuarios 
    WHERE email = p_email AND activo = TRUE;
    
    IF hashed_password = SHA2(p_password, 256) THEN

        SELECT id_usuario as id, nombre_usuario as username, email, activo as is_active
        FROM usuarios 
        WHERE email = p_email AND activo = TRUE;

        UPDATE usuarios 
        SET ultimo_login = CURRENT_TIMESTAMP 
        WHERE email = p_email;
    ELSE
        SELECT NULL AS id_usuario;
    END IF;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`bookstore`@`%` PROCEDURE `sp_obtener_resenas_usuario`(
    IN p_id_usuario INT,
    IN p_limite INT,
    IN p_offset INT
)
BEGIN
    SELECT 
        r.id_resena as idResena,
        r.calificacion,
        r.comentario,
        r.fecha_creacion as fechaResena,
        l.id_libro as idLibro,
        l.titulo,
        l.anio_publicacion as anioPublicacion,
        l.editorial,
        l.portada_url as portadaUrl
    FROM resenas r
    INNER JOIN libros l ON r.id_libro = l.id_libro
    WHERE r.id_usuario = p_id_usuario
    ORDER BY r.fecha_creacion DESC
    LIMIT p_limite OFFSET p_offset;

    SELECT COUNT(*) AS total
    FROM resenas
    WHERE id_usuario = p_id_usuario;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`bookstore`@`%` PROCEDURE `sp_agregar_resena`(
    IN p_id_libro INT,
    IN p_id_usuario INT,
    IN p_calificacion INT,
    IN p_comentario TEXT
)
BEGIN
    DECLARE usuario_existente INT;

    SELECT COUNT(*) INTO usuario_existente FROM usuarios 
    WHERE id_usuario = p_id_usuario AND activo = TRUE;
    
    IF usuario_existente > 0 THEN
        INSERT INTO resenas (id_libro, id_usuario, calificacion, comentario)
        VALUES (p_id_libro, p_id_usuario, p_calificacion, p_comentario);
        
        SELECT LAST_INSERT_ID() AS id_resena;
    ELSE
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'El usuario no existe o no está activo';
    END IF;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`bookstore`@`%` PROCEDURE `sp_actualizar_resena`(
    IN p_id_resena INT,        
    IN p_id_usuario INT,      
    IN p_nuevo_comentario TEXT, 
    IN p_nueva_calificacion INT 
)
BEGIN
    DECLARE v_owner_check INT DEFAULT 0;

    IF p_nueva_calificacion >= 1 AND p_nueva_calificacion <= 5 THEN

        SELECT COUNT(*) INTO v_owner_check
        FROM resenas
        WHERE id_resena = p_id_resena AND id_usuario = p_id_usuario;

        IF v_owner_check = 1 THEN
            UPDATE resenas
            SET
                comentario = p_nuevo_comentario,
                calificacion = p_nueva_calificacion

            WHERE
                id_resena = p_id_resena;

        ELSE
             SELECT 'Error: Reseña no encontrada o permiso denegado.' AS mensaje_error;
        END IF;

    ELSE
         SELECT 'Error: La calificación debe estar entre 1 y 5.' AS mensaje_error;
    END IF;

END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`bookstore`@`%` PROCEDURE `sp_obtener_top_libros_calificados`(
    IN p_limite INT
)
BEGIN
    SET p_limite = COALESCE(p_limite, 5);
    
    SELECT 
        l.id_libro as idLibro, 
        l.titulo, 
        l.portada_url as portadaUrl,
        a.id_autor, 
        l.anio_publicacion as anioPublicacion,
        a.id_autor idAutor,
        c.id_categoria idCategoria,
        CONCAT(a.nombre, ' ', a.apellido) AS autor,
        c.id_categoria, 
        c.nombre AS categoria,
        AVG(r.calificacion) AS rating,
        COUNT(r.id_resena) AS totalRating
    FROM 
        libros l
    JOIN 
        autores a ON l.id_autor = a.id_autor
    JOIN 
        categorias c ON l.id_categoria = c.id_categoria
    LEFT JOIN 
        resenas r ON l.id_libro = r.id_libro
    GROUP BY 
        l.id_libro
    HAVING 
        COUNT(r.id_resena) > 0 
    ORDER BY 
        rating DESC, 
        totalRating DESC     
    LIMIT p_limite;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`bookstore`@`%` PROCEDURE `sp_buscar_libros`(
    IN p_titulo VARCHAR(255),
    IN p_id_autor INT,
    IN p_id_categoria INT,
    IN p_limite INT,
    IN p_offset INT,
    IN p_orden VARCHAR(50),        
    IN p_direccion VARCHAR(4)      
)
BEGIN
    DECLARE orden_columna VARCHAR(50);
    DECLARE direccion_orden VARCHAR(4);

    SET orden_columna = CASE 
        WHEN p_orden = 'anio' THEN 'l.anio_publicacion'
        WHEN p_orden = 'rating' THEN 'rating'
        ELSE 'l.titulo'
    END;

    SET direccion_orden = CASE 
        WHEN UPPER(p_direccion) = 'DESC' THEN 'DESC'
        ELSE 'ASC'
    END;


    SET @sql = CONCAT(
        'SELECT l.id_libro AS idLibro, l.titulo, l.isbn, l.anio_publicacion AS anioPublicacion, l.resumen, l.portada_url AS portadaUrl,
                a.id_autor AS idAutor, CONCAT(a.nombre, '' '', a.apellido) AS autor,
                c.id_categoria AS idCategoria, c.nombre AS categoria,
                (SELECT AVG(calificacion) FROM resenas WHERE id_libro = l.id_libro) AS rating,
                (SELECT COUNT(*) FROM resenas WHERE id_libro = l.id_libro) AS totalRating
         FROM libros l
         JOIN autores a ON l.id_autor = a.id_autor
         JOIN categorias c ON l.id_categoria = c.id_categoria
         WHERE (', IF(p_titulo IS NULL OR p_titulo = '', '1=1', CONCAT('l.titulo LIKE ''%', p_titulo, '%''')), ')
         AND (', IF(p_id_autor IS NULL OR p_id_autor = 0, '1=1', CONCAT('l.id_autor = ', p_id_autor)), ')
         AND (', IF(p_id_categoria IS NULL OR p_id_categoria = 0, '1=1', CONCAT('l.id_categoria = ', p_id_categoria)), ')
         ORDER BY ', orden_columna, ' ', direccion_orden, '
         LIMIT ', p_limite, ' OFFSET ', p_offset, ';'
    );

    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;

    SELECT COUNT(*) AS total
    FROM libros l
    WHERE (p_titulo IS NULL OR p_titulo = '' OR l.titulo LIKE CONCAT('%', p_titulo, '%'))
      AND (p_id_autor IS NULL OR p_id_autor = 0 OR l.id_autor = p_id_autor)
      AND (p_id_categoria IS NULL OR p_id_categoria = 0 OR l.id_categoria = p_id_categoria);

END$$
DELIMITER ;




