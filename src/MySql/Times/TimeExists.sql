DROP procedure IF EXISTS `TimeExists`;

CREATE PROCEDURE `TimeExists` (
  IN CourseId BINARY(16),
  IN PlayerId BINARY(16)
)
BEGIN
  SELECT
    1
  FROM
    times
  WHERE
    times.CourseId = CourseId
    AND
    times.PlayerId = PlayerId;
END;
