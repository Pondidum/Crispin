import { fetch, addTask } from 'domain-task';

export const listAllToggles = () => (dispatch, getState) => {
    let fetchTask = fetch(`/toggles`)
        .then(response => response.json())
        .then(data => dispatch({
            type: 'RECEIVE_ALL_TOGGLES',
            toggles: data
        }))

    addTask(fetchTask)
    dispatch({ type: 'REQUEST_ALL_TOGGLES' })
}
