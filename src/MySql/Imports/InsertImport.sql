DROP procedure IF EXISTS `InsertImport`;

CREATE PROCEDURE `InsertImport` (
  IN Id BINARY(16),
  IN StartTime DATETIME
)
BEGIN
  INSERT INTO imports(
    Id,
    StartTime
  )
  VALUES(
    Id,
    StartTime
  );
END
