-- TODO: Deploy to the AWS.

-- TODO: Add a pipeline to deploy from GitHub.

CREATE TABLE IF NOT EXISTS `logins` (
  `Id` VARCHAR(32) NOT NULL,
  `Email` VARCHAR(255) NOT NULL,
  `PersonId` VARCHAR(32) NOT NULL,
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
