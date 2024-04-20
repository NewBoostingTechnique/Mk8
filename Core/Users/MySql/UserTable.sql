-- TODO: Rename 'User' -> 'Logon' (Person can have multiple logons).

-- TODO: Deduplicate player name with person name.
-- Keep the Player table, even if it just keys the person table.

CREATE TABLE IF NOT EXISTS `users` (
  `Id` VARCHAR(32) NOT NULL,
  `Email` VARCHAR(255) NOT NULL,
  `PersonId` VARCHAR(32) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Email_UNIQUE` (`Email` ASC) VISIBLE,
  INDEX `Email_INDEX` (`Email` ASC) VISIBLE,
  CONSTRAINT `User_Person`
    FOREIGN KEY (`PersonId`)
    REFERENCES `persons` (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;
