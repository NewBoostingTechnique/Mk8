CREATE TABLE IF NOT EXISTS `logins` (
  `Id` BINARY(16) NOT NULL,
  `Email` VARCHAR(255) NOT NULL,
  `PersonId` BINARY(16) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Email_UNIQUE` (`Email` ASC) VISIBLE,
  INDEX `Email_INDEX` (`Email` ASC) VISIBLE,
  CONSTRAINT `Login_Person`
    FOREIGN KEY (`PersonId`)
    REFERENCES `persons` (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;
