DROP procedure IF EXISTS `DetailImport`;

CREATE PROCEDURE `DetailImport`(
  IN Id BINARY(16)
)
BEGIN
  SELECT
    EndTime,
    Error,
    StartTime
  FROM
    imports
  WHERE
    imports.Id = Id;
END;
