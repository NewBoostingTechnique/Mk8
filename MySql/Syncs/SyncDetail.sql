DROP procedure IF EXISTS `SyncDetail`;

CREATE PROCEDURE `SyncDetail`(
  IN Id VARCHAR(32)
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
