CREATE TABLE IF NOT EXISTS `prooftypes` (
  `Id` BINARY(16) NOT NULL,
  `Description` VARCHAR(255) NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Description_UNIQUE` (`Description` ASC) VISIBLE,
  INDEX `Description_Index` (`Description` ASC) VISIBLE
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;
