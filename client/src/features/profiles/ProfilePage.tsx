import { Grid, Typography } from '@mui/material';
import { useParams } from 'react-router';

import ProfileHeader from './ProfileHeader';
import ProfileContent from './ProfileContent';

import { useProfile } from '../../lib/hooks/useProfile';

export default function ProfilePage() {
  const { id } = useParams();
  const { profile, loadingProfile } = useProfile(id);

  if (loadingProfile) return <Typography>Loading profile...</Typography>;

  if (!profile) return <Typography>Porfile not found</Typography>;
  return (
    <Grid>
      <Grid size={12}>
        <ProfileHeader profile={profile} />
        <ProfileContent />
      </Grid>
    </Grid>
  );
}
