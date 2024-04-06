CREATE TABLE IF NOT EXISTS `players` (
  `Id` VARCHAR(32) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  `CountryId` VARCHAR(32) NOT NULL,
  `ProofTypeId` VARCHAR(32) NOT NULL,
  `RegionId` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Name_UNIQUE` (`Name` ASC) VISIBLE,
  INDEX `Name` (`Name` ASC) VISIBLE,
  CONSTRAINT `Player_Country`
    FOREIGN KEY (`CountryId`)
    REFERENCES `countries` (`Id`),
  CONSTRAINT `Player_ProofType`
    FOREIGN KEY (`ProofTypeId`)
    REFERENCES `prooftypes` (`Id`),
  CONSTRAINT `Player_Region`
    FOREIGN KEY (`RegionId`)
    REFERENCES `regions` (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;