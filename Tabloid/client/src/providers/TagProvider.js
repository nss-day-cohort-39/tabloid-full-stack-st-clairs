import React, { useState, useContext } from "react";
import { UserProfileContext } from "./UserProfileProvider";

export const TagContext = React.createContext();

export const TagProvider = (props) => {

    const apiUrl = "/api/tag";

    const { getToken } = useContext(UserProfileContext);
    const [tags, setTags] = useState([]);

    const getAllTags = () =>
        getToken().then((token) =>
            fetch(apiUrl, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }).then(resp => resp.json())
                .then(setTags));

    const addTag = (tag) =>
        getToken().then((token) =>
            fetch(apiUrl, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(tag)
            }).then(resp => {
                if (resp.ok) {
                    return resp.json();
                }
                throw new Error("Unauthorized");
            })).then(getAllTags)

    const deleteTag = (id) =>
        getToken().then((token) =>
            fetch(`api/tag/${id}`, {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(id)
            })).then(getAllTags)

    const updateTag = (tag) =>
            getToken().then((token) =>
                fetch(`api/tag/${tag.id}`, {
                    method: "PUT",
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(tag)
                })).then(getAllTags)

    const getTag = (id) => {
        getToken().then((token) =>
            fetch(`/api/tag/${id}`, {
                method: "GET",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }))
            .then((res) => res.json())
    }
    return (
        <TagContext.Provider value={{ tags, getAllTags, addTag, getTag, deleteTag, updateTag }}>
            {props.children}
        </TagContext.Provider>
    );
};