DROP procedure IF EXISTS `SyncUpdate`;

CREATE PROCEDURE `SyncUpdate` (
  IN EndTime DATETIME,
  IN Id BINARY(16)
)
BEGIN
  UPDATE syncs
  SET EndTime = EndTime
  WHERE Id = Id;
END;
