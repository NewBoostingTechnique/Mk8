import { Link } from 'react-router-dom';

import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';

import ImportContactsIcon from '@mui/icons-material/ImportContacts';
import NewspaperIcon from '@mui/icons-material/Newspaper';
import PeopleIcon from '@mui/icons-material/People';
import PublishIcon from '@mui/icons-material/Publish';
import RuleIcon from '@mui/icons-material/Rule';

interface MenuProps {
  readonly authorization: boolean;
  readonly showProgress: () => void;
}

export default function Menu({ authorization, showProgress }: MenuProps) {
  return (
    <List className='menu' onClick={showProgress}>
      <ListItem key='News' disablePadding>
        <ListItemButton component={Link} to='/news/'>
          <ListItemIcon>
            <NewspaperIcon />
          </ListItemIcon>
          <ListItemText primary='News' />
        </ListItemButton>
      </ListItem>
      <ListItem key='Submit' disablePadding>
        <ListItemButton>
          <ListItemIcon>
            <PublishIcon />
          </ListItemIcon>
          <ListItemText primary='Submit' />
        </ListItemButton>
      </ListItem>
      <ListItem key='Rules' disablePadding>
        <ListItemButton component={Link} to='/rules/'>
          <ListItemIcon>
            <RuleIcon />
          </ListItemIcon>
          <ListItemText primary='Rules' />
        </ListItemButton>
      </ListItem>
      <ListItem key='Players' disablePadding>
        <ListItemButton component={Link} to='/players/'>
          <ListItemIcon>
            <PeopleIcon />
          </ListItemIcon>
          <ListItemText primary='Players' />
        </ListItemButton>
      </ListItem>
      {
        authorization
          ?
          <ListItem key='Migrations' disablePadding>
            <ListItemButton component={Link} to='/migrations/'>
              <ListItemIcon>
                <ImportContactsIcon />
              </ListItemIcon>
              <ListItemText primary='Migrations' />
            </ListItemButton>
          </ListItem>
          : null
      }
    </List>
  );
}
