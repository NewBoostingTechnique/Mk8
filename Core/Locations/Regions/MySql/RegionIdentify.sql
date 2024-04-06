DROP procedure IF EXISTS `RegionIdentify`;

DELIMITER $$
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
END$$

DELIMITER ;
