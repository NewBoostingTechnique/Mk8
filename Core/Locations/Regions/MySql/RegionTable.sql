CREATE TABLE IF NOT EXISTS `regions` (
  `Id` VARCHAR(32) NOT NULL,
  `CountryId` VARCHAR(32) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Region_Duplicate` (`CountryId` ASC, `Name` ASC) VISIBLE,
  INDEX `Name_INDEX` (`Name` ASC) VISIBLE,
  CONSTRAINT `Region_Country`
    FOREIGN KEY (`CountryId`)
    REFERENCES `countries` (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;
