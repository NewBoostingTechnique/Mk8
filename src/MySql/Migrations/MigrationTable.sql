CREATE TABLE IF NOT EXISTS `migration` (
  `Id` BINARY(16) NOT NULL,  
  `Description` VARCHAR(255) NOT NULL,
  `Progress` TINYINT NOT NULL,
  `Error` TEXT NULL,
  `StartTime` DATETIME NOT NULL,
  `EndTime` DATETIME NULL,
  PRIMARY KEY (`Id`)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- #region Create.

DROP PROCEDURE IF EXISTS `migration_create`;

CREATE PROCEDURE `migration_create` (
  IN Id BINARY(16),
  IN Description VARCHAR(255),
  IN Progress TINYINT,
  IN StartTime DATETIME
)
BEGIN
  INSERT INTO migration(
    Id,
    Description,
    Progress,
    StartTime
  )
  VALUES(
    Id,
    Description,
    Progress,
    StartTime
  );
END;

-- #endregion Create.

-- #region Detail.

DROP PROCEDURE IF EXISTS `migration_detail`;

CREATE PROCEDURE `migration_detail` (
  IN Id BINARY(16)
)
BEGIN
  SELECT
    Description,
    Progress,
    Error,
    StartTime,
    EndTime
  FROM
    migration
  WHERE
    migration.Id = Id;
END;

-- #endregion Detail.

-- #region Index.

DROP PROCEDURE IF EXISTS `migration_index`;

CREATE PROCEDURE `migration_index` ()
BEGIN
  SELECT
    Id,
    Description,
    Progress,
    Error,
    StartTime,
    EndTime
  FROM
    migration
  ORDER BY
    StartTime DESC;
END;

-- #endregion Index.

-- #region Update.

DROP PROCEDURE IF EXISTS `migration_update`;

CREATE PROCEDURE `migration_update` (
  IN Id BINARY(16),
  IN Progress TINYINT,
  IN Error TEXT,
  IN EndTime DATETIME
)
BEGIN
  UPDATE
    migration
  SET
    Progress = Progress,
    Error = Error,
    EndTime = EndTime
  WHERE
    Id = Id;
END;

-- #endregion Update.
