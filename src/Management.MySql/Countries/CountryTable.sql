CREATE TABLE IF NOT EXISTS `country`(
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

DROP PROCEDURE IF EXISTS `country_create`;

CREATE PROCEDURE `country_create` (
  IN Id BINARY(16),
  IN Name VARCHAR(255)
)
BEGIN
  INSERT INTO country(
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

DROP PROCEDURE IF EXISTS `country_identify`;

CREATE PROCEDURE `country_identify` (
  IN Name VARCHAR(255)
)
BEGIN
  SELECT
    country.Id
  FROM
    country
  WHERE
    country.Name = Name;
END;


-- #endregion Identify.

-- #region Index.

DROP PROCEDURE IF EXISTS `country_index`;

CREATE PROCEDURE `country_index` ()
BEGIN
  SELECT
    country.Name
  FROM
    country;
END;


-- #endregion Index.
