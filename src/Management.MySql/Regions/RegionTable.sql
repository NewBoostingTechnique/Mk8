CREATE TABLE IF NOT EXISTS `region` (
  `Id` BINARY(16) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  `CountryId` BINARY(16) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE INDEX `Region_Duplicate` (`CountryId` ASC, `Name` ASC) VISIBLE,
  INDEX `Name_INDEX` (`Name` ASC) VISIBLE,
  CONSTRAINT `Region_Country`
    FOREIGN KEY (`CountryId`)
    REFERENCES `country` (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- #region Create.

DROP PROCEDURE IF EXISTS `region_create`;

CREATE PROCEDURE `region_create` (
  IN Id BINARY(16),
  IN Name VARCHAR(255),
  IN CountryId BINARY(16)
)
BEGIN
  INSERT INTO region(
    Id,
    Name,
    CountryId
  )
  VALUES(
    Id,
    Name,
    CountryId
  );
END;

-- #endregion Create.

-- #region Identify.

DROP PROCEDURE IF EXISTS `region_identify`;

CREATE PROCEDURE `region_identify` (
  IN Name VARCHAR(255)
)
BEGIN
  SELECT
    region.Id
  FROM
    region
  WHERE
    region.Name = Name;
END;

-- #endregion Identify.

-- #region Index.

DROP PROCEDURE IF EXISTS `region_index`;

CREATE PROCEDURE `region_index` (
  IN CountryId BINARY(16)
)
BEGIN
  SELECT
    region.Name
  FROM
    region
  WHERE
    region.CountryId = CountryId;
END;

-- #endregion Index.
