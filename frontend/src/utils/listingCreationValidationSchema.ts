import * as Yup from "yup";

const SUPPORTED_FORMATS = [
  "image/jpeg",   
  "image/png", 
  "image/webp", 
  "image/gif", 
  "image/svg+xml", 
  "image/avif",    
  "image/heic", 
  "image/heif"
];
const FILE_SIZE_LIMIT = 5 * 1024 * 1024;

const fileSchema = Yup.mixed<File>()
  .nullable()
  .test("fileType", "Invalid file type", (value) => {
    if (!value) return true;
    return SUPPORTED_FORMATS.includes(value.type);
  })
  .test("fileSize", "File too large", (value) => {
    if (!value) return true;
    return value.size <= FILE_SIZE_LIMIT;
  });

export const listingCreationValidationSchema = Yup.object({
    title: Yup.string()
        .max(250, "Title cannot be longer then 250 chars")
        .required("Tittle is required"),
    
    price: Yup.number()
        .min(1, "Price can't be lower than 1$")
        .max(100_000, "Price can't be larger than 100.000$")
        .required("Price is required"),
    
    description: Yup.string()
        .max(1500, "Description length can't be over than 1500 chars")
        .required("Description is required"),
    
    categoryIds: Yup.array()
        .of(Yup.number().required())
        .min(1, "Choose at least 1 category")
        .required("Choose category"),

    images: Yup.array()
        .of(fileSchema)
        .min(1, "Add at least 1 image")
        .nullable(),
});
