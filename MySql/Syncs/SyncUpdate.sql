DROP procedure IF EXISTS `SyncUpdate`;

CREATE PROCEDURE `SyncUpdate` (
  IN EndTime DATETIME,
  IN Id VARCHAR(32)
)
BEGIN
  UPDATE syncs
  SET EndTime = EndTime
  WHERE Id = Id;
END;
