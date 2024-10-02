CREATE TABLE IF NOT EXISTS `person` (
  `Id` BINARY(16) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Name_UNIQUE` (`Name` ASC) VISIBLE,
  INDEX `Name_INDEX` (`Name` ASC) VISIBLE
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- #region Create.

DROP PROCEDURE IF EXISTS `person_create`;

CREATE PROCEDURE `person_create` (
  IN Id BINARY(16),
  IN Name VARCHAR(255)
)
BEGIN
  INSERT INTO person(
    Id,
    Name
  )
  VALUES(
    Id,
    Name
  );
END;

-- #endregion Create.

-- #region Identify.

DROP PROCEDURE IF EXISTS `person_identify`;

CREATE PROCEDURE `person_identify` (
  IN Name VARCHAR(255)
)
BEGIN
  SELECT
    person.Id
  FROM
    person
  WHERE
    person.Name = Name;
END;

-- #endregion Identify.
