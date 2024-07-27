CREATE TABLE IF NOT EXISTS `news` (
  `Id` BINARY(16) NOT NULL,
  `AuthorPersonId` BINARY(16) NOT NULL,
  `Body` TEXT NOT NULL,
  `Date` DATE NOT NULL,
  `Title` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`Id`),
  CONSTRAINT `News_Author`
    FOREIGN KEY (`AuthorPersonId`)
    REFERENCES `persons` (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;
