DROP procedure IF EXISTS `PlayerDelete`;

CREATE PROCEDURE `PlayerDelete` (
  IN Id BINARY(16)
)
BEGIN
  DELETE
  FROM    players
  WHERE   players.Id = Id;
END;
