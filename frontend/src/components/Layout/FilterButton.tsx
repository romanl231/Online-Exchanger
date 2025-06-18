interface FilterButtonProps {
  handleFilterClick?: () => void;
}

export const FilterButton: React.FC<FilterButtonProps> = ({
    handleFilterClick,
}) => {
    return (
        <button 
            type="button"
            className="bg-transparent hover:bg-transparent focus:bg-transparent" 
            onClick={handleFilterClick}>
            <img
                src="https://cdn.builder.io/api/v1/image/assets/TEMP/00051a37d1738bd649c2480085f98310266cb64b?placeholderIfAbsent=true&apiKey=12288f4bd46346b3b7e56474a7cea5ec"
                className="w-5 h-5 object-contain shrink-0"
                alt="Filter icon"
            />
        </button>
    );
}