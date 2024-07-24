DROP procedure IF EXISTS `RegionIdentify`;

CREATE PROCEDURE `RegionIdentify` (
  IN RegionName VARCHAR(255)
)
BEGIN
  SELECT
    regions.Id
  FROM
    regions
  WHERE
    regions.Name = RegionName;
END;
