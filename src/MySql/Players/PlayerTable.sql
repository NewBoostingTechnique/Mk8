CREATE TABLE IF NOT EXISTS `player` (
  `Id` BINARY(16) NOT NULL,
  `CountryId` BINARY(16) NOT NULL,  
  `RegionId` BINARY(16) NULL,
  PRIMARY KEY (`Id`),
  CONSTRAINT `Player_Person`
    FOREIGN KEY (`Id`)
    REFERENCES `person` (`Id`),
  CONSTRAINT `Player_Country`
    FOREIGN KEY (`CountryId`)
    REFERENCES `country` (`Id`),
  CONSTRAINT `Player_Region`
    FOREIGN KEY (`RegionId`)
    REFERENCES `region` (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- #region Create

DROP PROCEDURE IF EXISTS `player_create`;

CREATE PROCEDURE `player_create` (
  IN Id BINARY(16),
  IN CountryId BINARY(16),
  IN RegionId BINARY(16)
)
BEGIN
  INSERT INTO player(
    Id,
    CountryId,
    RegionId
  )
  VALUES(
    Id,
    CountryId,
    RegionId
  );
END;

-- #endregion Create

-- #region Detail

DROP PROCEDURE IF EXISTS `player_detail`;

CREATE PROCEDURE `player_detail` (
  IN Id BINARY(16)
)
BEGIN
  SELECT
    person.Name,
    MAX(time.Date) AS 'Active',
    country.Name AS 'CountryName',
    region.Name AS 'RegionName'
  FROM
    player
    JOIN
      country ON player.CountryId = country.Id
    JOIN
      person ON player.Id = person.Id
    JOIN
      region ON player.RegionId = region.Id
    LEFT OUTER JOIN
      time ON player.Id = time.PlayerId
  WHERE
    player.Id = Id
  GROUP BY
    player.Id;

  SELECT
    course.Name AS 'CourseName',
    time.Span
  FROM
    course
    LEFT OUTER JOIN
      time ON course.Id = time.CourseId
  WHERE
    time.PlayerId IS NULL
    OR
      time.PlayerId = Id;
END;

-- #endregion Detail

-- #region Delete.

DROP PROCEDURE IF EXISTS `player_delete`;

CREATE PROCEDURE `player_delete` (
  IN Id BINARY(16)
)
BEGIN
  DELETE
  FROM    player
  WHERE   ID IS NULL OR player.Id = Id;
END;

-- #endregion Delete.

-- #region Exists.

DROP PROCEDURE IF EXISTS `player_exists`;

CREATE PROCEDURE `player_exists` (
  IN Name VARCHAR(255)
)
BEGIN
  SELECT
    1
  FROM
    player
    JOIN
      person ON player.Id = person.Id
  WHERE
    person.Name = Name;
END;

-- #endregion Exists.

-- #region Identify.

DROP PROCEDURE IF EXISTS `player_identify`;

CREATE PROCEDURE `player_identify` (
  IN Name VARCHAR(255)
)
BEGIN
  SELECT
    player.Id
  FROM
    player
    JOIN
      person ON player.Id = person.Id
  WHERE
    person.Name = Name;
END;

-- #endregion Identify.

-- #region Index.

DROP PROCEDURE IF EXISTS `player_index`;

CREATE PROCEDURE `player_index` ()
BEGIN
  SELECT
    MAX(time.Date) AS 'Active',
    country.Name AS 'CountryName',
    person.Name AS 'Name',
    region.Name AS 'RegionName'
  FROM
    player
    JOIN
      country ON player.CountryId = country.Id
    JOIN
      person on player.Id = person.Id
    JOIN
      region ON player.RegionId = region.Id
    LEFT OUTER JOIN
      time ON player.Id = time.PlayerId
  GROUP BY
    player.Id;
END;

-- #endregion Index.
