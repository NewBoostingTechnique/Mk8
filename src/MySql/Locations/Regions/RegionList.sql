DROP procedure IF EXISTS `RegionList`;

CREATE PROCEDURE `RegionList` (
  IN CountryId BINARY(16)
)
BEGIN
  SELECT
    regions.Name
  FROM
    regions
  WHERE
    regions.CountryId = CountryId;
END;
