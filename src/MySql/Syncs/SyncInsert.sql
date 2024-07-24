DROP procedure IF EXISTS `SyncInsert`;

CREATE PROCEDURE `SyncInsert` (
  IN Id VARCHAR(32),
  IN StartTime DATETIME
)
BEGIN
  INSERT INTO syncs(
    Id,
    StartTime
  )
  VALUES(
    Id,
    StartTime
  );
END
