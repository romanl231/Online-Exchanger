interface FromInputFieldProps {
  minPrice: string;
  onChange: (value: string) => void;
}

export const FromInputField: React.FC<FromInputFieldProps> = (
    {
        minPrice,
        onChange,
    }
) => {

    return (
    <div className="mb-3">
            <label className="block text-sm font-medium text-[#EAEAEA]">From</label>
            <input
              type="number"
              value={minPrice}
              onChange={(e) => onChange(e.target.value)}
              className="w-full rounded-3xl border h-[41px] px-4 pr-10 
                        text-gray-200 outline-none bg-zinc-800"
              placeholder="Minimum price"
            />
          </div>);
}