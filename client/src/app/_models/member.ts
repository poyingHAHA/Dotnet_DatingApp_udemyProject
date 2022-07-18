import { Photo } from "./Photo";

export interface Member {
  id: number;
  username: string;
  age: number;
  knownAs: string;
  photoUrl: string;
  created: Date;
  lastActive: Date;
  gender: string;
  introduction: string;
  lookingFor: string;
  interests: string;
  country: string;
  photos: Photo[];
}
