import { List, ListItem, Stack, Typography } from "@mui/material"

export default function RuleList() {
  return (
    <Stack sx={{ gap: { xs: 2, md: 3 } }}>
      <Typography variant="h2">Rules</Typography>
      <Typography variant="body1">
        To ensure the integrity of our leaderboards, we must ensure that our users follow the site rules, as listed below. Any infraction to these rules, if confirmed by one of our moderators, may result in your account being frozen or submissions being removed, at the discretion of our staff. We don't mean to scare you off by this, because the rules are rather easy to follow and aren't an issue for most players, as long as you don't cheat or lie about your times.
        <List sx={{ listStyleType: 'disc' }}>
          <ListItem sx={{ display: 'list-item' }}>
            You may only submit times set on official Wii U or Switch systems, along with official copies of the game (physical or digital). Emulators, ROM hacks, and other devices are strictly forbidden.
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            Hacking of any kind (superspeed codes, moonjump codes, track/vehicle/character retexturing, music/sound effect modifications, etc.) is strictly forbidden.
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            Links to proof must show the game unmodified (recording runs with custom textures or other hacks is not permitted, even if the run was originally performed on an unmodified version of the game).
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            Cheating or lying about your times is forbidden.
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            You may not submit times that use glitches or substantial shortcuts not intended by the developers. For these purposes, "firehopping" and other hopping techniques are not considered glitches and are permitted.
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            Some sections of this site allow you to enter text which will be displayed publicly, such as profile messages and submission comments. Abusive/obscene language and any form of harassment of other people will not be tolerated.
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            You may not use URL shorteners when providing proof for your submissions. This is to help prevent spam or malicious links. Any submissions with URL-shortened proof will be rejected or removed.
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            Proof must follow the guidelines for proof on the Submission Help page.
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            You must submit times exactly as displayed by the in-game timer. Approximated/rounded times or times from an external timer are not permitted.
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            You must submit only your own times, and you may not submit times for other people. All proof provided must be your own.
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            You may not attempt to abuse the functionality of the site or take advantage of any errors or glitches in the programming.
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            You may only create one account, and you may not share an account or profile with another person.
          </ListItem>
          <ListItem sx={{ display: 'list-item' }}>
            The verifiers and moderators also reserve the right to ask you for proof if they are doubtful about your ability to obtain the time(s) you claim. They may freeze you (removing your option to submit new times) and/or remove some or all of your submissions if you do not comply to these rules or their requests.
          </ListItem>
        </List>
      </Typography>
    </Stack>
  );
}
