interface PublishButtonProps {
  disabled: boolean;
}

export function PublishButton({disabled}: PublishButtonProps) {
  return (
    <button
      type="submit"
      className= {`px-6 py-3 text-sm text-white 
                bg-purple-500 ${disabled ? "opacity-50 cursor-not-allowed" : ""} 
                rounded-full hover:bg-purple-600 transition 
                bg-purple-400 hover:bg-purple-500`}>
      Publish
    </button>
  );
}