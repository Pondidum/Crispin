
const defaultState = {
    loading: false,
    toggles: []
}

export default (state = defaultState, action) => {
    switch (action.type) {
        case 'REQUEST_ALL_TOGGLES':
            return {
                loading: true,
                toggles: []
            }
        
        case 'RECEIVE_ALL_TOGGLES':
            return {
                loading: false,
                toggles: action.toggles
            }
        
        default:
            return state;
    }
}