DROP procedure IF EXISTS `UpdateImport`;

CREATE PROCEDURE `UpdateImport` (
  IN EndTime DATETIME,
  IN Error TEXT,
  IN Id BINARY(16)
)
BEGIN
  UPDATE imports
  SET
    EndTime = EndTime,
    Error = Error
  WHERE Id = Id;
END;
