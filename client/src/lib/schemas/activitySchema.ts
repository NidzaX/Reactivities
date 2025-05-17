import { z } from 'zod';

//prettier-ignore
const requiredString = (fieldName: string) =>
  z.string({ required_error: `${fieldName} is required` })
   .min(1, { message: `${fieldName} is required` });

export const activitySchema = z.object({
  title: requiredString('title'),
  description: requiredString('description'),
  category: requiredString('category'),
  date: requiredString('date'),
  city: requiredString('city'),
  venue: requiredString('venue'),
});
