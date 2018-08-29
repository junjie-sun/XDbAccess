CREATE TABLE `org` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `user` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Birthday` datetime NOT NULL,
  `Description` text,
  `OrgId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `orguser` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Birthday` datetime NOT NULL,
  `Description` text,
  `OrgId` int(11) NOT NULL,
  `OrgName` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `user2` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) NOT NULL,
  `Birthday` datetime NOT NULL,
  `Description` text,
  `OrgId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE DEFINER = `root`@`localhost` PROCEDURE `ExecuteSqlTestSP`(IN `Name` varchar(50))
BEGIN
	INSERT INTO Org(`Name`) VALUES(`Name`);
END;
