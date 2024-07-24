DROP procedure IF EXISTS `RegionList`;

CREATE PROCEDURE `RegionList` (
  IN CountryId VARCHAR(32)
)
BEGIN
  SELECT
    regions.Name
  FROM
    regions
  WHERE
    regions.CountryId = CountryId;
END;
