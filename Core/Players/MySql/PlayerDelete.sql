DROP procedure IF EXISTS `PlayerDelete`;

DELIMITER $$
CREATE PROCEDURE `PlayerDelete` (
  IN PlayerId VARCHAR(32)
)
BEGIN
  DELETE
  FROM    players
  WHERE   Id = PlayerId;
END$$

DELIMITER ;

