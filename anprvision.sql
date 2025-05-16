-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 16-05-2025 a las 18:21:21
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
-- Estructura de tabla para la tabla `blacklist`
--

CREATE TABLE `blacklist` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `reason` varchar(255) NOT NULL COMMENT 'TRIAL',
  `restrictiondate` date NOT NULL COMMENT 'TRIAL',
  `id_client` int(11) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial473` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `blacklist`
--

INSERT INTO `blacklist` (`id`, `reason`, `restrictiondate`, `id_client`, `active`, `trial473`) VALUES
(15, 'deja la moto tirada', '2025-04-23', 4, 1, 'T'),
(16, 'dejo', '2025-04-23', 5, 0, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `camara`
--

CREATE TABLE `camara` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `nightvisioninfrared` tinyint(1) NOT NULL COMMENT 'TRIAL',
  `highresolution` tinyint(1) NOT NULL COMMENT 'TRIAL',
  `infraredlighting` tinyint(1) NOT NULL COMMENT 'TRIAL',
  `name` varchar(50) NOT NULL COMMENT 'TRIAL',
  `optimizedangleofvision` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `highshutterspeed` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial477` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `camara`
--

INSERT INTO `camara` (`id`, `nightvisioninfrared`, `highresolution`, `infraredlighting`, `name`, `optimizedangleofvision`, `highshutterspeed`, `active`, `trial477`) VALUES
(8, 1, 1, 1, 'hola', 1, 1, 1, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `client`
--

CREATE TABLE `client` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `id_user` int(11) DEFAULT NULL COMMENT 'TRIAL',
  `name` varchar(50) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial470` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `client`
--

INSERT INTO `client` (`id`, `id_user`, `name`, `active`, `trial470`) VALUES
(4, NULL, 'yerson', 1, 'T'),
(5, NULL, 'German', 1, 'T');

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
-- Estructura de tabla para la tabla `invoice`
--

CREATE TABLE `invoice` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `totalamount` decimal(10,2) NOT NULL COMMENT 'TRIAL',
  `paymentstatus` varchar(50) NOT NULL COMMENT 'TRIAL',
  `paymentdate` datetime NOT NULL COMMENT 'TRIAL',
  `id_vehiclehistory` int(11) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial496` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `memberships`
--

CREATE TABLE `memberships` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `membershiptype` varchar(50) NOT NULL COMMENT 'TRIAL',
  `startdate` date NOT NULL COMMENT 'TRIAL',
  `enddate` date NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial500` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `memberships`
--

INSERT INTO `memberships` (`id`, `membershiptype`, `startdate`, `enddate`, `active`, `trial500`) VALUES
(2, 'semanal', '2025-04-10', '2025-04-10', 1, 'T'),
(4, 'semanal', '2025-04-23', '2025-04-23', 0, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `membershipsvehicle`
--

CREATE TABLE `membershipsvehicle` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `id_memberships` int(11) NOT NULL COMMENT 'TRIAL',
  `id_vehicle` int(11) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial503` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

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
-- Estructura de tabla para la tabla `parking`
--

CREATE TABLE `parking` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `name` varchar(50) NOT NULL COMMENT 'TRIAL',
  `location` varchar(50) NOT NULL COMMENT 'TRIAL',
  `id_camara` int(11) NOT NULL COMMENT 'TRIAL',
  `hability` longtext NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial503` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `parking`
--

INSERT INTO `parking` (`id`, `name`, `location`, `id_camara`, `hability`, `active`, `trial503`) VALUES
(10, 'asdasdas', 'sdfsafd', 8, 'sadfsdaf', 1, 'T');

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
-- Estructura de tabla para la tabla `rates`
--

CREATE TABLE `rates` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `amount` decimal(10,2) NOT NULL COMMENT 'TRIAL',
  `startduration` longtext NOT NULL COMMENT 'TRIAL',
  `id_typerates` int(11) NOT NULL COMMENT 'TRIAL',
  `endduration` longtext NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial506` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `rates`
--

INSERT INTO `rates` (`id`, `amount`, `startduration`, `id_typerates`, `endduration`, `active`, `trial506`) VALUES
(4, 500.00, '05:00:00', 2, '06:00:00', 1, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `registeredvehicle`
--

CREATE TABLE `registeredvehicle` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `entrydatetime` time DEFAULT NULL COMMENT 'TRIAL',
  `exitdatetime` time DEFAULT NULL COMMENT 'TRIAL',
  `id_vehicle` int(11) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial496` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

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
-- Estructura de tabla para la tabla `typerates`
--

CREATE TABLE `typerates` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `name` varchar(50) NOT NULL COMMENT 'TRIAL',
  `price` decimal(10,2) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial506` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `typerates`
--

INSERT INTO `typerates` (`id`, `name`, `price`, `active`, `trial506`) VALUES
(2, 'Noche', 5.00, 1, 'T'),
(3, 'Día', 3.50, 1, 'T'),
(4, 'Fin de semana', 6.00, 1, 'T'),
(5, 'Noche', 5.00, 1, 'T'),
(6, 'Día', 3.50, 1, 'T'),
(7, 'Fin de semana', 6.00, 1, 'T'),
(8, 'Noche', 5.00, 1, 'T'),
(9, 'Día', 3.50, 1, 'T'),
(10, 'Fin de semana', 6.00, 1, 'T'),
(11, 'Noche', 5.00, 1, 'T'),
(12, 'Día', 3.50, 1, 'T'),
(13, 'Fin de semana', 6.00, 1, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `typevehicles`
--

CREATE TABLE `typevehicles` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `name` varchar(50) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial490` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `typevehicles`
--

INSERT INTO `typevehicles` (`id`, `name`, `active`, `trial490`) VALUES
(2, 'me voy ', 1, 'T');

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

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `vehicle`
--

CREATE TABLE `vehicle` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `plate` varchar(50) NOT NULL COMMENT 'TRIAL',
  `color` varchar(100) DEFAULT NULL COMMENT 'TRIAL',
  `id_client` int(11) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial493` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Volcado de datos para la tabla `vehicle`
--

INSERT INTO `vehicle` (`id`, `plate`, `color`, `id_client`, `active`, `trial493`) VALUES
(6, 'sadasd', 'red', 5, 1, 'T');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `vehiclehistory`
--

CREATE TABLE `vehiclehistory` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `totaltime` time NOT NULL COMMENT 'TRIAL',
  `id_typevehicle` int(11) NOT NULL COMMENT 'TRIAL',
  `id_registeredvehicle` int(11) NOT NULL COMMENT 'TRIAL',
  `id_invoice` int(11) DEFAULT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial500` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `vehiclehistoryparkingrates`
--

CREATE TABLE `vehiclehistoryparkingrates` (
  `id` int(11) NOT NULL COMMENT 'TRIAL',
  `hourused` int(11) NOT NULL COMMENT 'TRIAL',
  `id_rates` int(11) NOT NULL COMMENT 'TRIAL',
  `id_vehiclehistory` int(11) NOT NULL COMMENT 'TRIAL',
  `id_parking` int(11) NOT NULL COMMENT 'TRIAL',
  `subtotal` decimal(10,2) NOT NULL COMMENT 'TRIAL',
  `active` tinyint(1) DEFAULT NULL COMMENT 'TRIAL',
  `trial509` char(1) DEFAULT NULL COMMENT 'TRIAL'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='TRIAL';

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `blacklist`
--
ALTER TABLE `blacklist`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_client` (`id_client`);

--
-- Indices de la tabla `camara`
--
ALTER TABLE `camara`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `client`
--
ALTER TABLE `client`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `client_id_user_key` (`id_user`);

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
-- Indices de la tabla `invoice`
--
ALTER TABLE `invoice`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_vehiclehistory` (`id_vehiclehistory`);

--
-- Indices de la tabla `memberships`
--
ALTER TABLE `memberships`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `membershipsvehicle`
--
ALTER TABLE `membershipsvehicle`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_memberships` (`id_memberships`),
  ADD KEY `fk_vehicle_1` (`id_vehicle`);

--
-- Indices de la tabla `module`
--
ALTER TABLE `module`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `parking`
--
ALTER TABLE `parking`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_camaras` (`id_camara`);

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
-- Indices de la tabla `rates`
--
ALTER TABLE `rates`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_typerates` (`id_typerates`);

--
-- Indices de la tabla `registeredvehicle`
--
ALTER TABLE `registeredvehicle`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_vehicle` (`id_vehicle`);

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
-- Indices de la tabla `typerates`
--
ALTER TABLE `typerates`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `typevehicles`
--
ALTER TABLE `typevehicles`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `user_username_key` (`username`),
  ADD KEY `fk_person` (`id_person`);

--
-- Indices de la tabla `vehicle`
--
ALTER TABLE `vehicle`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `vehicle_plate_key` (`plate`),
  ADD KEY `fk_client_1` (`id_client`);

--
-- Indices de la tabla `vehiclehistory`
--
ALTER TABLE `vehiclehistory`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_typevehicle` (`id_typevehicle`),
  ADD KEY `fk_registeredvehicle` (`id_registeredvehicle`),
  ADD KEY `fk_vehiclehistory_type` (`id_invoice`);

--
-- Indices de la tabla `vehiclehistoryparkingrates`
--
ALTER TABLE `vehiclehistoryparkingrates`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_rates` (`id_rates`),
  ADD KEY `fk_vehiclehistory_1` (`id_vehiclehistory`),
  ADD KEY `fk_parking` (`id_parking`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `blacklist`
--
ALTER TABLE `blacklist`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `camara`
--
ALTER TABLE `camara`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `client`
--
ALTER TABLE `client`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=6;

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
-- AUTO_INCREMENT de la tabla `invoice`
--
ALTER TABLE `invoice`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL';

--
-- AUTO_INCREMENT de la tabla `memberships`
--
ALTER TABLE `memberships`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `membershipsvehicle`
--
ALTER TABLE `membershipsvehicle`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL';

--
-- AUTO_INCREMENT de la tabla `module`
--
ALTER TABLE `module`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `parking`
--
ALTER TABLE `parking`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=11;

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
-- AUTO_INCREMENT de la tabla `rates`
--
ALTER TABLE `rates`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `registeredvehicle`
--
ALTER TABLE `registeredvehicle`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL';

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
-- AUTO_INCREMENT de la tabla `typerates`
--
ALTER TABLE `typerates`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `typevehicles`
--
ALTER TABLE `typevehicles`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `user`
--
ALTER TABLE `user`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=37;

--
-- AUTO_INCREMENT de la tabla `vehicle`
--
ALTER TABLE `vehicle`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL', AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `vehiclehistory`
--
ALTER TABLE `vehiclehistory`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL';

--
-- AUTO_INCREMENT de la tabla `vehiclehistoryparkingrates`
--
ALTER TABLE `vehiclehistoryparkingrates`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'TRIAL';

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `blacklist`
--
ALTER TABLE `blacklist`
  ADD CONSTRAINT `fk_client` FOREIGN KEY (`id_client`) REFERENCES `client` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `client`
--
ALTER TABLE `client`
  ADD CONSTRAINT `fk_user` FOREIGN KEY (`id_user`) REFERENCES `user` (`id`) ON DELETE SET NULL ON UPDATE NO ACTION;

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
-- Filtros para la tabla `invoice`
--
ALTER TABLE `invoice`
  ADD CONSTRAINT `fk_vehiclehistory` FOREIGN KEY (`id_vehiclehistory`) REFERENCES `vehiclehistory` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `membershipsvehicle`
--
ALTER TABLE `membershipsvehicle`
  ADD CONSTRAINT `fk_memberships` FOREIGN KEY (`id_memberships`) REFERENCES `memberships` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_vehicle_1` FOREIGN KEY (`id_vehicle`) REFERENCES `vehicle` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `parking`
--
ALTER TABLE `parking`
  ADD CONSTRAINT `fk_camaras` FOREIGN KEY (`id_camara`) REFERENCES `camara` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `rates`
--
ALTER TABLE `rates`
  ADD CONSTRAINT `fk_typerates` FOREIGN KEY (`id_typerates`) REFERENCES `typerates` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `registeredvehicle`
--
ALTER TABLE `registeredvehicle`
  ADD CONSTRAINT `fk_vehicle` FOREIGN KEY (`id_vehicle`) REFERENCES `vehicle` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;

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

--
-- Filtros para la tabla `vehicle`
--
ALTER TABLE `vehicle`
  ADD CONSTRAINT `fk_client_1` FOREIGN KEY (`id_client`) REFERENCES `client` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `vehiclehistory`
--
ALTER TABLE `vehiclehistory`
  ADD CONSTRAINT `fk_registeredvehicle` FOREIGN KEY (`id_registeredvehicle`) REFERENCES `registeredvehicle` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_typevehicle` FOREIGN KEY (`id_typevehicle`) REFERENCES `typevehicles` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_vehiclehistory_type` FOREIGN KEY (`id_invoice`) REFERENCES `invoice` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;

--
-- Filtros para la tabla `vehiclehistoryparkingrates`
--
ALTER TABLE `vehiclehistoryparkingrates`
  ADD CONSTRAINT `fk_parking` FOREIGN KEY (`id_parking`) REFERENCES `parking` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_rates` FOREIGN KEY (`id_rates`) REFERENCES `rates` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_vehiclehistory_1` FOREIGN KEY (`id_vehiclehistory`) REFERENCES `vehiclehistory` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
