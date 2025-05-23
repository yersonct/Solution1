-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 23-05-2025 a las 11:39:16
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `anprvision`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `formmodule`
--

CREATE TABLE `formmodule` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `id_module` int(11) NOT NULL COMMENT 'TRIAL',
  `id_forms` int(11) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial480` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `formmodule`
--

INSERT INTO `formmodule` (`id`, `id_module`, `id_forms`, `active`, `trial480`) VALUES
(5, 3, 2, 1, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `formrolpermission`
--

CREATE TABLE `formrolpermission` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `id_forms` int(11) NOT NULL COMMENT 'TRIAL',
  `id_rol` int(11) NOT NULL COMMENT 'TRIAL',
  `id_permission` int(11) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial486` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `formrolpermission`
--

INSERT INTO `formrolpermission` (`id`, `id_forms`, `id_rol`, `id_permission`, `active`, `trial486`) VALUES
(15, 2, 3, 1, 0, 'T'),
(16, 5, 6, 3, 1, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `forms`
--

CREATE TABLE `forms` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `name` varchar(100) NOT NULL COMMENT 'TRIAL',
  `url` varchar(255) DEFAULT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial477` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `forms`
--

INSERT INTO `forms` (`id`, `name`, `url`, `active`, `trial477`) VALUES
(2, 'volver', 'hola', 0, 'T'),
(5, 'yerson', 'asfdasd', 1, 'T'),
(6, 'sharon ramos', 'hola', 1, 'T'),
(7, 'pepe', 'sdsad', 0, 'T'),
(8, 'pepe', 'sdsad', 1, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `logs`
--

CREATE TABLE `logs` (
  `Id` int(11) NOT NULL,
  `Message` text DEFAULT NULL,
  `MessageTemplate` text DEFAULT NULL,
  `Level` varchar(128) DEFAULT NULL,
  `TimeStamp` datetime DEFAULT current_timestamp(),
  `Exception` text DEFAULT NULL,
  `Properties` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`Properties`)),
  `LogEvent` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_bin DEFAULT NULL CHECK (json_valid(`LogEvent`))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `module`
--

CREATE TABLE `module` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `name` varchar(100) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial477` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `module`
--

INSERT INTO `module` (`id`, `name`, `active`, `trial477`) VALUES
(3, 'uuuuuuuuuuuuuuuuuuuuuuuuuuu', 1, 'T'),
(5, 'hola', 1, 'T'),
(6, 'hola', 0, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `permission`
--

CREATE TABLE `permission` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `name` varchar(255) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial483` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `permission`
--

INSERT INTO `permission` (`id`, `name`, `active`, `trial483`) VALUES
(1, 'salir', 1, 'T'),
(3, 'Crear', 1, 'T'),
(4, 'Leer', 0, 'T'),
(5, 'Actualizar', 1, 'T'),
(6, 'Eliminar', 1, 'T'),
(7, 'Exportar', 1, 'T'),
(8, 'Importar', 0, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `person`
--

CREATE TABLE `person` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `name` varchar(50) NOT NULL COMMENT 'TRIAL',
  `document` varchar(20) NOT NULL COMMENT 'TRIAL',
  `phone` varchar(20) DEFAULT NULL COMMENT 'TRIAL',
  `lastname` varchar(50) NOT NULL COMMENT 'TRIAL',
  `email` varchar(100) DEFAULT NULL COMMENT 'TRIAL',
  `active` tinyint(1) NOT NULL COMMENT 'TRIAL',
  `trial460` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `person`
--

INSERT INTO `person` (`id`, `name`, `document`, `phone`, `lastname`, `email`, `active`, `trial460`) VALUES
(23, 'lucas', '7894561230', '9876543214', 'ramiro', 'lucas@gmail.com', 1, 'T'),
(24, 'rober', '898798484', '498497895', 'lili', 'rober@gmail.com', 1, 'T'),
(25, 'pepe', '789456123', '789456123', 'ramirez', 'pepe@gmail.com', 1, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `rol`
--

CREATE TABLE `rol` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `name` varchar(30) NOT NULL COMMENT 'TRIAL',
  `description` longtext DEFAULT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial483` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `rol`
--

INSERT INTO `rol` (`id`, `name`, `description`, `active`, `trial483`) VALUES
(3, 'Operador', 'Acceso limitado a funciones operativas', 1, 'T'),
(4, 'sdfsafsa', 'sadfsadf', 0, 'T'),
(5, 'lider', 'mandon', 1, 'T'),
(6, 'lider', 'si', 1, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `roluser`
--

CREATE TABLE `roluser` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `id_rol` int(11) NOT NULL COMMENT 'TRIAL',
  `id_user` int(11) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial509` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `roluser`
--

INSERT INTO `roluser` (`id`, `id_rol`, `id_user`, `active`, `trial509`) VALUES
(5, 6, 7, 1, 'T'),
(6, 5, 9, 1, 'T'),
(7, 5, 17, 1, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `user`
--

CREATE TABLE `user` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `username` varchar(50) NOT NULL COMMENT 'TRIAL',
  `password` varchar(255) NOT NULL COMMENT 'TRIAL',
  `id_person` int(11) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial467` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `user`
--

INSERT INTO `user` (`id`, `username`, `password`, `id_person`, `active`, `trial467`) VALUES
(34, 'lucas', '$2a$11$j2mytaRa/0gwJ4Qw2JzEu.3xqSQboDZ2cpEumY9vJMgbaG0aEiJIK', 23, 1, 'T'),
(35, 'rober', '$2a$11$/auCAyhG0WeOfX7FS4zVKeeqghmtQjuPcRGgkXQrvv2xA0LMdkPwu', 24, 1, 'T'),
(36, 'pepe', '$2a$11$0EloMdwvwZHsvpbVRkx69uJkvDdtCw.5Xm3dackvdvwsrGQH1XJBy', 25, 1, 'T');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `formmodule`
--
ALTER TABLE `formmodule`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_module` (`id_module`),
  ADD KEY `fk_forms` (`id_forms`);

--
-- Indices de la tabla `formrolpermission`
--
ALTER TABLE `formrolpermission`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_role` (`id_rol`),
  ADD KEY `fk_forms_1` (`id_forms`),
  ADD KEY `fk_permission` (`id_permission`);

--
-- Indices de la tabla `forms`
--
ALTER TABLE `forms`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `logs`
--
ALTER TABLE `logs`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `module`
--
ALTER TABLE `module`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `permission`
--
ALTER TABLE `permission`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `person`
--
ALTER TABLE `person`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `person_document_key` (`document`),
  ADD UNIQUE KEY `person_email_key` (`email`);

--
-- Indices de la tabla `rol`
--
ALTER TABLE `rol`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `roluser`
--
ALTER TABLE `roluser`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_role_1` (`id_rol`);

--
-- Indices de la tabla `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `user_username_key` (`username`),
  ADD KEY `fk_person` (`id_person`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `formmodule`
--
ALTER TABLE `formmodule`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `formrolpermission`
--
ALTER TABLE `formrolpermission`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `forms`
--
ALTER TABLE `forms`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `logs`
--
ALTER TABLE `logs`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `module`
--
ALTER TABLE `module`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `permission`
--
ALTER TABLE `permission`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `person`
--
ALTER TABLE `person`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=26;

--
-- AUTO_INCREMENT de la tabla `rol`
--
ALTER TABLE `rol`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `roluser`
--
ALTER TABLE `roluser`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=37;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `formmodule`
--
ALTER TABLE `formmodule`
  ADD CONSTRAINT `fk_forms` FOREIGN KEY (`id_forms`) REFERENCES `forms` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_module` FOREIGN KEY (`id_module`) REFERENCES `module` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `formrolpermission`
--
ALTER TABLE `formrolpermission`
  ADD CONSTRAINT `fk_forms_1` FOREIGN KEY (`id_forms`) REFERENCES `forms` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_permission` FOREIGN KEY (`id_permission`) REFERENCES `permission` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_role` FOREIGN KEY (`id_rol`) REFERENCES `rol` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `roluser`
--
ALTER TABLE `roluser`
  ADD CONSTRAINT `fk_role_1` FOREIGN KEY (`id_rol`) REFERENCES `rol` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `fk_person` FOREIGN KEY (`id_person`) REFERENCES `person` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
