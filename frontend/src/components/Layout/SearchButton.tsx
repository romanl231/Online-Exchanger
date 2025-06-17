interface SearchButtonProps {

}

export const SearchButton : React.FC<SearchButtonProps> = () =>{
    return (
        <button className="bg-transparent hover:bg-transparent focus:bg-transparent">
            <img
                src="https://cdn.builder.io/api/v1/image/assets/TEMP/4272772a03f465985b36fe6e67db6fb3e49023f2?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec"
                className="w-5 h-5 object-contain shrink-0"
                alt="Search icon"
            />
        </button>
    );
}