import React from 'react';
import './MainDashboard.css';

interface SearchBarProps {
    searchTerm: string;
    onSearchChange: (term: string) => void;
}

const SearchBar: React.FC<SearchBarProps> = ({ searchTerm, onSearchChange }) => {
    return (
        <div className="search-container">
            <input
                type="text"
                placeholder="Search orders..."
                value={searchTerm}
                onChange={(e) => onSearchChange(e.target.value)}
            />
        </div>
    );
};

export default SearchBar;