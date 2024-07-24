DROP procedure IF EXISTS `TimeInsert`;

CREATE PROCEDURE `TimeInsert` (
  IN CourseId VARCHAR(32),
  IN PlayerId VARCHAR(32),
  In TimeDate DATE,
  IN TimeId VARCHAR(32),
  IN TimeSpan TIME(3)
)
BEGIN
  INSERT INTO times(
    Id,
    CourseId,
    Date,
    PlayerId,
    TimeSpan
  )
  VALUES(
    TimeId,
    CourseId,
    TimeDate,
    PlayerId,
    TimeSpan
  );
END;
