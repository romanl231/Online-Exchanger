interface AddImageButtonProps {
    handleClick: () => void;
    isActive: boolean;
}

export function AddImageButton({handleClick, isActive}: AddImageButtonProps)  {
    return (
        <button
            disabled={isActive}
            onClick={handleClick}
            className={`px-6 py-3 text-sm text-white 
                bg-purple-500 rounded-full hover:bg-purple-600 transition 
                ${!isActive ? "bg-purple-400 hover:bg-purple-500" : "opacity-50 cursor-not-allowed bg-purple-400"}`}
          >
            Add photo
          </button>
    );
}