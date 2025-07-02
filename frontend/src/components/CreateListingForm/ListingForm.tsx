import { ImageUploadSection } from "./ImageUploadSection";
import { TitleSection } from "./TitleSection";
import { CategoriesSection } from "./CategoriesSection";
import { PriceSection } from "./PriceSection";
import { DescriptionSection } from "./DescriptionSection";
import { PublishButton } from "./PublishButton";
import { ListingService } from "../../api/listingApi";
import { toast } from "react-toastify";
import { getApiErrorMessage } from "../../utils/getApiErrorMessage";
import { useFormik } from "formik";
import { listingCreationValidationSchema } from "../../utils/listingCreationValidationSchema";

export default function ListingForm() {

  const formik = useFormik({
    initialValues: {
      title: '',
      price: 0,
      description: '',
      categoryIds: [],
      images: [] as File[],
    },
    validationSchema: listingCreationValidationSchema,
    onSubmit: async (values) => {
      console.log("SUBMIT VALUES:", values);
      try {
          await ListingService.create(values);
          toast.success("Listing successfuly created", {
            position: "top-center",
            autoClose: 5000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            theme: "dark",
            });
        } catch (err) {
          toast.error(getApiErrorMessage(err), {
            position: "top-center",
            autoClose: 5000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            theme: "dark",
        });
      }
    }
  })

  return (
    <div className="flex justify-center items-start w-full px-4 py-10">
      <form 
        className="w-full bg-neutral-800 rounded-2xl p-8 shadow-lg"
        onSubmit={formik.handleSubmit}>
        <div className="flex-col gap-6">
          <ImageUploadSection
            images={formik.values.images}
            setImages={(file: File[]) => formik.setFieldValue("images", file)} 
            titleText="Upload images"/>
          <TitleSection
            title={formik.values.title}
            setTitle={(val) => formik.setFieldValue("title", val)} 
            titleText="Create title"/>
          <CategoriesSection
            selectedCategoryIds={formik.values.categoryIds}
            setSelectedCategoryIds={(val) => formik.setFieldValue("categoryIds", val)} 
            titleText="Choose categories"/>
          <PriceSection
            price={formik.values.price}
            setPrice={(val) => formik.setFieldValue("price", val)} 
            titleText="Select price"/>
          <DescriptionSection
            description={formik.values.description}
            setDescription={(val) => formik.setFieldValue("description", val)} 
            titleText="Description"/>
          <div className="py-10 flex justify-center">
            <PublishButton />
          </div>
        </div>
      </form>
    </div>
  );
}