import { Box, Button, Paper, Typography } from '@mui/material';
import { useForm } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router';
import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';

import { useActivities } from '../../../lib/hooks/useActivities';
import { activitySchema, type ActivitySchema } from '../../../lib/schemas/activitySchema';
import TextInput from '../../../app/shared/components/TextInput';
import SelectInput from '../../../app/shared/components/SelectInput';
import { categoryOptions } from './categoryOptions';
import DateTimeInput from '../../../app/shared/components/DateTimeInput';
import LocationInput from '../../../app/shared/components/LocationInput';

export default function ActivityForm() {
  const { reset, handleSubmit, control } = useForm<ActivitySchema>({
    mode: 'onTouched',
    resolver: zodResolver(activitySchema),
  });
  const navigate = useNavigate();
  const { id } = useParams();
  const { updateActivity, createActivity, activity, isLoading } = useActivities(id);

  useEffect(() => {
    if (activity)
      reset({
        ...activity,
        date: new Date(activity.date),
        location: {
          city: activity.city,
          venue: activity.venue,
          latitude: activity.latitude,
          longitude: activity.longitude,
        },
      });
  }, [activity, reset]);

  const onSubmit = async (data: ActivitySchema) => {
    const { location, ...rest } = data;
    const flattenedData = {
      ...rest,
      ...location,
      date: rest.date.toISOString(),
    };

    console.log('Final data being sent:', flattenedData);

    try {
      if (activity) {
        updateActivity.mutate(
          { ...activity, ...flattenedData },
          {
            onSuccess: () => navigate(`/activities/${activity.id}`),
          },
        );
      } else {
        createActivity.mutate(flattenedData, {
          onSuccess: (id) => navigate(`/activities/${id}`),
        });
        console.log(flattenedData);
      }
    } catch (error) {
      console.log(error);
    }
  };

  if (isLoading) return <Typography>Loading activity...</Typography>;

  return (
    <Paper sx={{ borderRadius: 3, padding: 3 }}>
      <Typography variant="h5" gutterBottom color="primary">
        {activity ? 'Edit activity' : 'Create activity'}
      </Typography>
      <Box onSubmit={handleSubmit(onSubmit)} component="form" display="flex" flexDirection="column" gap={3}>
        <TextInput label="Title" control={control} name="title" />
        <TextInput label="Description" control={control} name="description" multiline rows={3} />
        <Box display="flex" gap={3}>
          <SelectInput items={categoryOptions} label="Category" control={control} name="category" />
          <DateTimeInput label="Date" control={control} name="date" />
        </Box>

        <LocationInput control={control} label="Enter the location" name="location" />

        <Box display="flex" justifyContent="end" gap={3}>
          <Button color="inherit">Cancel</Button>
          <Button
            type="submit"
            color="success"
            variant="contained"
            disabled={updateActivity.isPending || createActivity.isPending}
          >
            Submit
          </Button>
        </Box>
      </Box>
    </Paper>
  );
}
