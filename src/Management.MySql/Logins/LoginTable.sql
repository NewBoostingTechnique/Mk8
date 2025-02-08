CREATE TABLE IF NOT EXISTS `login` (
  `Id` BINARY(16) NOT NULL,
  `Email` VARCHAR(255) NOT NULL,
  `PersonId` BINARY(16) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Email_UNIQUE` (`Email` ASC) VISIBLE,
  INDEX `Email_INDEX` (`Email` ASC) VISIBLE,
  CONSTRAINT `Login_Person`
    FOREIGN KEY (`PersonId`)
    REFERENCES `person` (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- #region Create.

DROP PROCEDURE IF EXISTS `login_create`;

CREATE PROCEDURE `login_create` (
  IN Id BINARY(16),  
  IN Email VARCHAR(255),
  IN PersonId BINARY(16)
)
BEGIN
  INSERT INTO login(
    Id,
    Email,
    PersonId
  )
  VALUES(
    Id,
    Email,
    PersonId
  );
END;

-- #endregion Create.

-- #region Exists.

DROP PROCEDURE IF EXISTS `login_exists`;

CREATE PROCEDURE `login_exists` (
  IN Email VARCHAR(255)
)
BEGIN
  SELECT
    1
  FROM
    login
  WHERE
    login.Email = Email;
END;


-- #endregion Exists.
