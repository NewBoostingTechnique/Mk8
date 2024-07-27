CREATE TABLE IF NOT EXISTS `players` (
  `CountryId` BINARY(16) NOT NULL,
  `Id` BINARY(16) NOT NULL,
  `ProofTypeId` BINARY(16) NOT NULL,
  `RegionId` BINARY(16) NOT NULL,
  PRIMARY KEY (`Id`),
  CONSTRAINT `Player_Country`
    FOREIGN KEY (`CountryId`)
    REFERENCES `countries` (`Id`),
  CONSTRAINT `Player_Person`
    FOREIGN KEY (`Id`)
    REFERENCES `persons` (`Id`),
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
