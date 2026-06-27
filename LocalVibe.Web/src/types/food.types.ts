export interface RelatedMeal {
  name: string;
  category: string | null;
  area: string | null;
  thumbnailUrl: string | null;
  instructions: string | null;
  youtubeUrl: string | null;
  sourceUrl: string | null;
}

export interface FoodInfoResponse {
  postId: number;
  postTitle: string;
  relatedMeals: RelatedMeal[];
  provider: string;
}
