DROP procedure IF EXISTS `TimeExists`;

CREATE PROCEDURE `TimeExists` (
  IN CourseId VARCHAR(32),
  IN PlayerId VARCHAR(32)
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
