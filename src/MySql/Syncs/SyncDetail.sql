DROP procedure IF EXISTS `SyncDetail`;

CREATE PROCEDURE `SyncDetail`(
  IN Id BINARY(16)
)
BEGIN
  SELECT
    EndTime,
    StartTime
  FROM
    syncs
  WHERE
    syncs.Id = Id;
END;
