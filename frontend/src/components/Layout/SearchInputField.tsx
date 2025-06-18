interface SearchButtonProps {
    placeholder?: string;
    setQuery: (value: string) => void;
}

export const SearchInputField : React.FC<SearchButtonProps> = ({
    setQuery,
    placeholder = "What are you looking for?",
}) =>{
    return (
            <input
                    type="text"
                    onChange={(e) => setQuery(e.target.value)}
                    placeholder={placeholder}
                    className="w-full bg-transparent outline-none text-gray-200 placeholder-gray-400"
                />
    );
}